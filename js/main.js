
/*jshint esversion: 6 */
(function($, window, document) {

    var Colours = Object.freeze({
        "BOARD_LIGHT": "#e8e8e8",
        "BOARD_DARK": "#333333",
        "CHECKER_RED": "#ba0000",
        "CHECKER_BLACK": "#000000",
    })

    // Square State
    var SQState = Object.freeze({
        "EMPTY": 0,
        "WHITE": 1,
        "BLACK": 2
    })

    var board = [
        [SQState.BLACK, SQState.BLACK, SQState.BLACK, SQState.BLACK],
        [SQState.BLACK, SQState.BLACK, SQState.BLACK, SQState.BLACK],
        [SQState.BLACK, SQState.BLACK, SQState.BLACK, SQState.BLACK],
        [SQState.EMPTY, SQState.EMPTY, SQState.EMPTY, SQState.EMPTY],
        [SQState.EMPTY, SQState.EMPTY, SQState.EMPTY, SQState.EMPTY],
        [SQState.WHITE, SQState.WHITE, SQState.WHITE, SQState.WHITE],
        [SQState.WHITE, SQState.WHITE, SQState.WHITE, SQState.WHITE],
        [SQState.WHITE, SQState.WHITE, SQState.WHITE, SQState.WHITE]
    ];

    function drawSquareState(ctx, x, y, squareState) {
        if (squareState === SQState.EMPTY) return;

        sqsize = canvas.height/8.0;

        // Offset the x value for even rows
        x *= 2;
        x += ((y % 2) === 0) ? 1 : 0;

        ctx.fillStyle = (squareState === SQState.BLACK) ? Colours.CHECKER_BLACK : Colours.CHECKER_RED;
        ctx.beginPath();
        ctx.arc(
            x * sqsize + sqsize/2,
            y * sqsize + sqsize/2,
            sqsize * 0.38,
            0,
            2*Math.PI
        );
        ctx.stroke();
        ctx.fill();
    }

    $(function() {
        var c = document.getElementById("canvas");
        var ctx = c.getContext("2d");

        // Render board background
        ctx.fillStyle = Colours.BOARD_DARK;
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        ctx.fillStyle = Colours.BOARD_LIGHT;
        size = canvas.height/8.0;
        for (y = 0; y < 8; y++) {
            for (x = y & 1; x < 8; x += 2) {
                ctx.fillRect(x * size, y * size, size, size);
            }
        }

        // Render board state
        for (y = 0; y < 8; y++) {
            for (x = 0; x < 4; x++) {
                drawSquareState(ctx, x, y, board[y][x]);
            }
        }

    });

}(window.jQuery, window, document));
