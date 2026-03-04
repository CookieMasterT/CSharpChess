import {InitChessBoard} from "./boardManager.js";

const url = "ws://localhost:54321/";
let connection;
const supportedRequests = ["identification", "boardState", "pieceMoves", "movePiece", "currentTeam"];

export async function SendData(type, extraInfo = "") {
  if (supportedRequests.includes(type)) {
    let request = JSON.stringify({requestType: type, extraInfo: extraInfo});
    connection.send(request);
  } else {
    console.error(`The type '${type}' is not in supportedInfos`);
  }
}

export async function WaitForInfo(type, extraInfo = "") {
  let Task = new Promise(resolve => {
    connection.addEventListener("message",
      (e) => resolve(e.data)
      , {once: true})
  });
  await SendData(type, extraInfo);
  let returnedInfo;
  returnedInfo = await Task;
  return JSON.parse(returnedInfo);
}

async function CommandHandler(eventText) {
  switch (eventText) {
    case "refreshBoard":
      await InitChessBoard();
  }
}

export async function SetUpConnection() {
  connection = new WebSocket(url);

  connection.addEventListener("error", () => {
    document.getElementById("title").innerHTML = `Cannot connect to server (is it turned on?)`;
    document.getElementById("ChessBoard").classList.add("unavailable");
  });

  if (connection.readyState !== WebSocket.OPEN) {
    let Task = new Promise(resolve => {
      connection.addEventListener("open", resolve, {once: true});
    });
    await Task;
  }

  let sessionId = (await cookieStore.get("sessionIdentifier"))
  if (!sessionId) {
    let newId;
    newId = (await WaitForInfo("identification", "NoID"))["Id"];
    await cookieStore.set({name: "sessionIdentifier", value: newId, path: "/"});
  } else {
    await SendData("identification", sessionId.value);
  }
  let Task = new Promise(resolve => {
    connection.addEventListener("message", resolve, {once: true});
  });
  await Task

  connection.addEventListener("message", (event) => {
    CommandHandler(event.data);
  })
}
