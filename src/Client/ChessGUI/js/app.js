const url = "ws://localhost:54321/";
let connection;
const supportedRequests = ["identification", "boardState", "pieceMoves", "movePiece", "currentTeam"];

async function SendData(type, extraInfo = "") {
  if (supportedRequests.includes(type)) {
    let request = JSON.stringify({requestType: type, extraInfo: extraInfo})
    connection.send(request);
  } else {
    console.log(`The type '${type}' is not in supportedInfos`);
  }
}

async function WaitForInfo(type, extraInfo = "") {
  let Task = new Promise(resolve => {
    connection.addEventListener("message",
      (e) => resolve(e.data)
      , {once: true})
  });
  await SendData(type, extraInfo);
  let returnedInfo;
  returnedInfo = await Task;
  return JSON.parse(returnedInfo)
}

async function CommandHandler(eventText) {
  switch (eventText) {
    case "refreshBoard":
      await InitChessBoard()
  }
}

async function SetUpConnection() {
  connection = new WebSocket(url);
  let Task = new Promise(resolve => {
    connection.addEventListener("open", resolve, {once: true})
  });
  await Task

  let sessionId = (await cookieStore.get("sessionIdentifier"))
  if (!sessionId) {
    let newId;
    newId = (await WaitForInfo("identification", "NoID"))["Id"];
    await cookieStore.set({name: "sessionIdentifier", value: newId, path: "/"});
  } else {
    await SendData(sessionId);
  }

  connection.addEventListener("message", (event) => {
    CommandHandler(event.data);
  })
}

async function Init() {
  await SetUpConnection()
  await InitChessBoard();
}

const FileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
const RankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];

const ImageFileNameDict = {"K": "king", "Q": "queen", "R": "rook", "B": "bishop", "N": "knight", "P": "pawn"}

let TempListeners = []

async function InitChessBoard() {
  let board = await WaitForInfo("boardState")
  BuildChessBoard(board);

  let team = await WaitForInfo("currentTeam");
  document.getElementById("title").innerHTML = `It is currently: ${team["team"]}'s turn`;
  AddPieceInteractivity(team)
}

function BuildChessBoard(board_array) {
  console.log(board_array);
  let board = document.getElementById("ChessBoard")
  let html = "";

  html += `<table>`
  for (let i = 0; i < 10; i++) {
    html += `<tr>`
    for (let k = 0; k < 10; k++) {
      if (k !== 0 && i !== 0 && i !== 9 && k !== 9) {
        let x = k - 1;
        let y = i - 1;
        html += `<td
                  class=\"tile-${(x % 2 + y % 2) % 2 === 1 ? "w" : "b"}"
                  id="tile-${x};${y}">`

        let tile = board_array[x][y]
        if (tile !== "U") {
          let color = tile[0]
          let type;
          if (tile.length === 1)
            type = "P"
          else
            type = tile[1]

          html += `<img
                    class="piece ${color === 'w' ? 'White' : 'Black'}"
                    draggable="false"
                    id="piece-${x};${y}"
                    src="img/${ImageFileNameDict[type]}-${color}.svg" alt="${tile}"/>`;
        }
        html += `</td>`
      } else {
        html += `<td class="tile-edge">`
        if (i !== 0 && i !== 9) {
          html += `<span>${RankNames[i - 1]}</span>`
        } else {
          if (k !== 0 && k !== 9) {
            html += `<span>${FileNames[k - 1]}</span>`
          }
        }
        html += `</td>`
      }
    }
    html += `</tr>`
  }
  html += `</table>`

  board.innerHTML = html;
}

function AddPieceInteractivity(team) {
  let Pieces = document.querySelectorAll(`.piece.${team["team"]}`);
  for (let piece of Pieces) {
    piece.addEventListener('mousedown', (e) => {
      HandlePieceTouch(e).then()
    })
  }
}

async function HandlePieceTouch(event) {
  let PieceId = event.target.id;
  let [PieceX, PieceY] = PieceId.slice(-3).split(';');

  document.querySelectorAll(".MoveHintHighlight").forEach((item) => {
    item.classList.remove("MoveHintHighlight");
    item.removeEventListener('click', DoMove)
  })

  while (TempListeners.length > 0) {
    let [Element, Func] = TempListeners.pop()
    Element.removeEventListener('click', Func)
  }

  let request = await WaitForInfo("pieceMoves", `{"x": ${PieceX}, "y": ${PieceY}}`);
  request.forEach((tile) => {
    let [tileX, tileY] = tile;
    let tileElement = document.getElementById(`tile-${tileX};${tileY}`);
    tileElement.classList.add("MoveHintHighlight");
    let func = () => {
      DoMove(PieceX, PieceY, tileX, tileY)
    }
    tileElement.addEventListener('click', func)
    TempListeners.push([tileElement, func])
  })
}

async function DoMove(pieceX, pieceY, tileX, tileY) {
  console.log(pieceX, pieceY, tileX, tileY)
  await SendData("movePiece", `{"startX": ${pieceX}, "startY": ${pieceY},
                                                "endX": ${tileX}, "endY": ${tileY}}`);
}

Init().then();
