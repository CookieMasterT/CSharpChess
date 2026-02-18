const connectionString = "http://localhost:54321/";
const supportedInfos = ["boardState"];

async function RequestInfo(type) {
  if (supportedInfos.includes(type)) {
    let response;

    response = await fetch(
      connectionString,
      {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        signal: AbortSignal.timeout(5000),
        body: JSON.stringify({infoRequest: type})
      }
    );

    if (!response.ok) {
      throw new Error(`Server responded with status: ${response.status}`);
    }

    return await response.json();
  }
}

async function Init() {
  await InitChessBoard();
}

const FileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
const RankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];

const ImageFileNameDict = {"K": "king", "Q": "queen", "R": "rook", "B": "bishop", "N": "knight", "P":"pawn"}

async function InitChessBoard() {
  let request = await RequestInfo("boardState")
  let board = document.getElementById("ChessBoard")
  let html = "";

  html += `<table>`
  for (let i = 0; i < 10; i++) {
    html += `<tr>`
    for (let k = 0; k < 10; k++) {
      if (k !== 0 && i !== 0 && i !== 9 && k !== 9) {
        let x = k - 1;
        let y = i - 1;
        html += `<td class=\"tile-${(x % 2 + y % 2) % 2 === 1 ? "w" : "b"}">`

        let tile = request[x][y]
        if (tile !== "U") {
          let color = tile[0]
          let type;
          if (tile.length === 1)
            type = "P"
          else
            type = tile[1]

          html += `<img id="piece-${x};${y}" src="img/${ImageFileNameDict[type]}-${color}.svg" alt="${tile}"/>`;
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

Init().then();
