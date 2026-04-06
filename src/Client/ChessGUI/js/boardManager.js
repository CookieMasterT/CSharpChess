import {SendData, WaitForInfo} from "./webSocket.js";

let bodyEvent;

const FileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
const RankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];

const ImageFileNameDict = {"K": "king", "Q": "queen", "R": "rook", "B": "bishop", "N": "knight", "P": "pawn"};

let TempListeners = [];

export async function InitChessBoard() {
  let board = await WaitForInfo("boardState")
  BuildChessBoard(board["board"]);
  ShowMoveHistory(board["moveHistory"]);
  let team = board["currentTeam"];

  document.getElementById("title").innerHTML = `It is currently: ${team}'s turn`;
  AddPieceInteractivity(team);
}

export function AddPieceDragging() {
  window.addEventListener("mousemove", (e) => {
    if (document.querySelector(".dragging") != null) {
      document.querySelector(".dragging").style.left = `${e.clientX - 25}px`;
      document.querySelector(".dragging").style.top = `${e.clientY - 25}px`;
    }
  })
}

export function BuildChessBoard(board_array) {
  document.body.style.cursor = "";
  let board = document.getElementById("ChessBoard");
  let html = "";

  html += `<table>`;
  for (let i = 0; i < 10; i++) {
    html += `<tr>`;
    for (let k = 0; k < 10; k++) {
      if (k !== 0 && i !== 0 && i !== 9 && k !== 9) {
        let x = k - 1;
        let y = i - 1;
        html += `<td
                  class=\"tile-${(x % 2 + y % 2) % 2 === 1 ? "w" : "b"}"
                  id="tile-${x};${y}">`;

        if (board_array != null) {
          let tile = board_array[x][y];
          if (tile !== "U") {
            let color = tile[0];
            let type;
            if (tile.length === 1)
              type = "P";
            else
              type = tile[1];

            html += `<img
                    class="piece ${color === 'w' ? 'White' : 'Black'} ${type}"
                    draggable="false"
                    id="piece-${x};${y}"
                    src="img/${ImageFileNameDict[type]}-${color}.svg" alt="${tile}"/>`;
          }
        }
        html += `</td>`;
      } else {
        html += `<td class="tile-edge">`;
        if (i !== 0 && i !== 9) {
          html += `<span>${RankNames[i - 1]}</span>`;
        } else {
          if (k !== 0 && k !== 9) {
            html += `<span>${FileNames[k - 1]}</span>`;
          }
        }
        html += `</td>`;
      }
    }
    html += `</tr>`;
  }
  html += `</table>`;

  board.innerHTML = html;
}

function ShowMoveHistory(move_array) {
  let list = document.getElementById("MoveHistory");
  let html = "<table class=\"chess-history-table\">\n";

  // Iterate through the array in steps of 2 (White and Black)
  for (let i = 0; i < move_array.length; i += 2) {
    let moveNumber = (i / 2) + 1;
    let whiteMove = move_array[i];
    // If the array has an odd number of moves, the last black move will be empty
    let blackMove = move_array[i + 1] || "";

    html += "  <tr>\n";
    html += `    <td class="move-number">${moveNumber}.</td>\n`;
    html += `    <td class="move-white">${whiteMove}</td>\n`;
    html += `    <td class="move-black">${blackMove}</td>\n`;
    html += "  </tr>\n";
  }

  html += "</table>";
  list.innerHTML = html;
}

function AddPieceInteractivity(team) {
  let Pieces = document.querySelectorAll(`.piece.${team}`);
  for (let piece of Pieces) {
    piece.addEventListener('mousedown', (e) => {
      e.stopPropagation();
      HandlePieceTouch(e).then()
    })
    piece.classList.add("active");
  }
  if (!bodyEvent) {
    bodyEvent = true;
    window.addEventListener("mousedown", () => RemoveHintHighlights())
  }
}

async function HandlePieceTouch(event) {
  let Piece = event.target;
  let PieceId = Piece.id;
  let [PieceX, PieceY] = PieceId.slice(-3).split(';');

  RemoveHintHighlights();

  let request = await WaitForInfo("pieceMoves", `{"x": ${PieceX}, "y": ${PieceY}}`);
  document.getElementById(`tile-${PieceX};${PieceY}`).classList.add("move-hint-origin");
  request.forEach((tile) => {
    let [tileX, tileY] = tile;
    let tileElement = document.getElementById(`tile-${tileX};${tileY}`);
    tileElement.classList.add("move-hint-highlight");

    let needsPromotion = event.target.classList.contains("P") && (tileY === 0 || tileY === 7);
    let team = Piece.classList.contains("White") ? "w" : "b"

    let Func = (e) => {
      if (needsPromotion) {
        Piece.classList.remove("dragging");
        Piece.style.display = "none";
        RemoveHintHighlights(true);
        document.body.style.cursor = "";
      }
      DoMove(PieceX, PieceY, tileX, tileY, needsPromotion, team);
      e.stopPropagation();
    }

    AddTempListener(tileElement, Func, 'mouseup');
    AddTempListener(tileElement, Func, 'mousedown');
  })

  Piece.classList.add("dragging");
  document.body.style.cursor = "grabbing";
  Piece.style.left = `${event.clientX - 25}px`;
  Piece.style.top = `${event.clientY - 25}px`;
  window.addEventListener("mouseup", () => {
    Piece.classList.remove("dragging");
    document.body.style.cursor = "";
  })
}

function AddTempListener(Element, Func, Type) {
  Element.addEventListener(Type, Func);
  TempListeners.push([Element, Func, Type]);
}

function RemoveHintHighlights(partial = false) {
  document.querySelectorAll(".move-hint-highlight").forEach((item) => {
    item.classList.remove("move-hint-highlight");
  })
  if (!partial) {
    document.querySelectorAll(".move-hint-origin").forEach((item) => {
      item.classList.remove("move-hint-origin");
    })
  }

  while (TempListeners.length > 0) {
    let [Element, Func, Type] = TempListeners.pop();
    Element.removeEventListener(Type, Func);
  }
}

async function DoMove(pieceX, pieceY, tileX, tileY, needsPromotion, team) {
  if (needsPromotion) {
    let promotionPiece = await ChoosePromotionPiece(tileX, tileY, team)
    if (promotionPiece === "") {
      await InitChessBoard();
      return;
    }
    await SendData("movePieceWithPromotion", `{"startX": ${pieceX}, "startY": ${pieceY},
                                                "endX": ${tileX}, "endY": ${tileY},
                                                "promotionPiece": "${promotionPiece}"}`)
  } else {
    await SendData("movePiece", `{"startX": ${pieceX}, "startY": ${pieceY},
                                                "endX": ${tileX}, "endY": ${tileY}}`);
  }
}

function ChoosePromotionPiece(x, y, team) {
  return new Promise((resolve) => {
    let dialogue = document.getElementById("PromotionChoice");

    let pieces = {
      "Q": ImageFileNameDict["Q"],
      "R": ImageFileNameDict["R"],
      "B": ImageFileNameDict["B"],
      "N": ImageFileNameDict["N"]
    };

    let html = `<div class="promotion-menu">`;
    for (let [choice, pieceName] of Object.entries(pieces)) {
      html += `
        <button class="promo-btn" data-choice="${choice}">
          <img src="img/${pieceName}-${team}.svg" alt="${pieceName}" />
        </button>
      `;
    }

    html += `
        <button class="promo-btn cancel-btn" data-choice="">&times;</button>
      </div>
    `;

    dialogue.innerHTML = html

    let targetTile = document.getElementById(`tile-${x};${y}`).getBoundingClientRect();
    dialogue.style.display = "block";

    if (y < 4) {
      document.querySelector(".promotion-menu").style.flexDirection = "column";
      dialogue.style.left = targetTile.left + "px";
      dialogue.style.top = targetTile.top + "px";
    } else {
      document.querySelector(".promotion-menu").style.flexDirection = "column-reverse";
      dialogue.style.left = targetTile.left + "px";
      dialogue.style.top = (targetTile.top - 182) + "px";
    }

    let buttons = dialogue.querySelectorAll('.promo-btn');

    let cleanup = () => {
      buttons.forEach(btn => btn.removeEventListener('click', clickHandler));
      window.removeEventListener('mousedown', outsideClickHandler);
      dialogue.innerHTML = "";
      dialogue.style.display = "none";
    };

    let clickHandler = (event) => {
      const choice = event.currentTarget.getAttribute('data-choice');
      cleanup();
      resolve(choice);
    };

    let outsideClickHandler = (event) => {
      if (!dialogue.contains(event.target)) {
        cleanup();
        resolve("");
      }
    };

    buttons.forEach(btn => btn.addEventListener('click', clickHandler));
    window.addEventListener('mousedown', outsideClickHandler);
    let Pieces = document.querySelectorAll(`.piece`);
    for (let piece of Pieces) {
      piece.style.pointerEvents = "none";
    }
  });
}
