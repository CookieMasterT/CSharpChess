import {SetUpConnection} from "./webSocket.js";
import {InitChessBoard, AddPieceDragging, BuildChessBoard} from "./boardManager.js";

async function Init() {
  BuildChessBoard(null)
  await SetUpConnection();
  await InitChessBoard();
  AddPieceDragging();
}

Init().then();
