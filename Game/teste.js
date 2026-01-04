class Game {
  players = [];
  table = [
    [null, null, null],
    ["o", null, "o"],
    [null, null, null],
  ];
  turn;

  constructor() {
    this.players.push(new Player("Alice", "x"));
    sendPlayer(this.players[0]);
    this.turn = this.players[0];
  }

  getTable() {
    return this.table;
  }

  addPlayer(name) {
    if (this.players[0].value === "x") {
      this.players.push(new Player(name, "o"));

      sendPlayer(this.players[1]);

      return;
    }

    this.players.push(new Player(name, "x"));

    sendPlayer(this.players[1]);
  }

  makeMove(playerId, x, y) {
    if (this.players.length < 2) {
      throw new Error("Not enough players");
    }

    const player = this.players.find((player) => player.id === playerId);

    if (!player) {
      throw new Error("Player not found");
    }

    if (this.turn.id !== playerId) {
      throw new Error("Not your turn");
    }

    if (this.table[x][y] !== null) {
      throw new Error("Cell already occupied");
    }

    this.table[x][y] = player.value;

    this.checkWinner();
    this.hasDraw();
    this.changeTurn();

    return {
      table: this.table,
      turn: this.turn.id,
    };
  }

  changeTurn() {
    if (this.players.length < 2) {
      throw new Error("Not enough players to change turn");
    }

    if (this.turn === this.players[0]) {
      this.turn = this.players[1];
    }

    this.turn = this.players[0];
  }

  checkWinner() {
    for (let i = 0; i < 3; i++) {
      if (isEqual(this.table[i][0], this.table[i][1], this.table[i][2])) {
        sendWinnerNotification(this.turn.id, [
          [i, 0],
          [i, 1],
          [i, 2],
        ]);
        this.reset();

        return;
      }

      if (isEqual(this.table[0][i], this.table[1][i], this.table[2][i])) {
        sendWinnerNotification(this.turn.id, [
          [0, i],
          [1, i],
          [2, i],
        ]);
        this.reset();

        return;
      }
    }

    if (isEqual(this.table[0][0], this.table[1][1], this.table[2][2])) {
      sendWinnerNotification(this.turn.id, [
        [0, 0],
        [1, 1],
        [2, 2],
      ]);
      this.reset();

      return;
    }

    if (isEqual(this.table[0][2], this.table[1][1], this.table[2][0])) {
      sendWinnerNotification(this.turn.id, [
        [0, 2],
        [1, 1],
        [2, 0],
      ]);
      this.reset();

      return;
    }

    return null;
  }

  hasDraw() {
    for (let i = 0; i < 3; i++) {
      for (let j = 0; j < 3; j++) {
        if (this.table[i][j] === null) {
          return false;
        }
      }
    }

    sendDrawNotification();
    this.reset();

    return true;
  }

  reset() {
    this.table = [
      [null, null, null],
      [null, null, null],
      [null, null, null],
    ];

    this.turn = this.players[0];

    setTimeout(() => {
      console.log("Game has been reset.");
    }, 1000);

    return {
      table: this.table,
      turn: this.turn.id,
    };
  }
}

function isEqual(a, b, c) {
  return a === b && b === c;
}

function sendDrawNotification() {
  console.log("The game ended in a draw.");
}

function sendWinnerNotification(playerId, winningBlocks) {
  console.log(
    "Sending winner notification to player:",
    playerId,
    "with winning blocks:",
    winningBlocks
  );
}

function sendPlayer(player) {
  return player;
}

class Player {
  id;
  name;
  value;

  constructor(name, value) {
    this.id = Math.random().toString(36).substring(2, 15);
    this.name = name;
    this.value = value;
  }
}

const game = new Game();
game.addPlayer("Bob");

const alice = game.players[0];
const bob = game.players[1];

console.log(game.makeMove(alice.id, 1, 1));
