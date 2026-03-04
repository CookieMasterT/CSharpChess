import {SendData, WaitForInfo} from "./webSocket.js";

let bodyEvent;

const FileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
const RankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];

const ImageFileNameDict = {"K": "king", "Q": "queen", "R": "rook", "B": "bishop", "N": "knight", "P": "pawn"}

let TempListeners = []

export async function InitChessBoard() {
  let board = await WaitForInfo("boardState")
  BuildChessBoard(board);

  let team = await WaitForInfo("currentTeam");
  document.getElementById("title").innerHTML = `It is currently: ${team["team"]}'s turn`;
  AddPieceInteractivity(team)
}

function BuildChessBoard(board_array) {
  // console.log("Building chessboard with: ", board_array);
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
      e.stopPropagation();
      HandlePieceTouch(e).then()
    })
    piece.classList.add("active");
  }
  if (!bodyEvent) {
    bodyEvent = true;
    document.body.addEventListener("mousedown", () => RemoveHintHighlights())
  }
}

async function HandlePieceTouch(event) {
  let Piece = event.target;
  let PieceId = Piece.id;
  let [PieceX, PieceY] = PieceId.slice(-3).split(';');

  RemoveHintHighlights();

  let request = await WaitForInfo("pieceMoves", `{"x": ${PieceX}, "y": ${PieceY}}`);
  request.forEach((tile) => {
    let [tileX, tileY] = tile;
    let tileElement = document.getElementById(`tile-${tileX};${tileY}`);
    tileElement.classList.add("move-hint-highlight");
    let func = () => {
      DoMove(PieceX, PieceY, tileX, tileY)
    }
    tileElement.addEventListener('mouseup', func)
    TempListeners.push([tileElement, func, 'mouseup'])
    tileElement.addEventListener('mousedown', func)
    TempListeners.push([tileElement, func, 'mousedown'])

    func = (e) => {
      e.stopPropagation()
    };
    tileElement.addEventListener('mousedown', func)
    TempListeners.push([tileElement, func, 'mousedown'])
  })

  Piece.classList.add("dragging")
  Piece.style.left = `${event.clientX - 25}px`
  Piece.style.top = `${event.clientY - 25}px`
  window.addEventListener("mouseup", () => {
    Piece.classList.remove("dragging")
  })
}

export function AddPieceDragging() {
  window.addEventListener("mousemove", (e) => {
    if (document.querySelector(".dragging") != null) {
      document.querySelector(".dragging").style.left = `${e.clientX - 25}px`;
      document.querySelector(".dragging").style.top = `${e.clientY - 25}px`;
    }
  })
}

function RemoveHintHighlights() {
  document.querySelectorAll(".move-hint-highlight").forEach((item) => {
    item.classList.remove("move-hint-highlight");
  })

  while (TempListeners.length > 0) {
    let [Element, Func, Type] = TempListeners.pop()
    Element.removeEventListener(Type, Func)
  }
}

async function DoMove(pieceX, pieceY, tileX, tileY) {
  console.log(`Moving from: ${pieceX}, ${pieceY} to: ${tileX}, ${tileY}`)
  await SendData("movePiece", `{"startX": ${pieceX}, "startY": ${pieceY},
                                                "endX": ${tileX}, "endY": ${tileY}}`);
}
