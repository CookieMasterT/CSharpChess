import {SetUpConnection} from "./webSocket.js";
import {InitChessBoard, AddPieceDragging} from "./boardManager.js";

async function Init() {
  await SetUpConnection();
  await InitChessBoard();
  AddPieceDragging();
}

Init().then();
