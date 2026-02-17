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

    return await response.text();
  }
}

async function Init() {
  await InitChessBoard();
}

async function InitChessBoard() {
  let request = await RequestInfo("boardState")
  console.log(request);
}

Init().then();
