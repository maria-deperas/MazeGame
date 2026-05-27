using MazeGame.Core;
using MazeGame.UI.Panels;

namespace MazeGame.UI
{
    public class ConsoleGameView : IGameView
    {
        private readonly ScreenRenderer _renderer;
        private readonly IRenderable _leftPanel;
        private readonly IRenderable _middlePanel;
        private readonly IRenderable _rightPanel;

        public ConsoleGameView(
            IRenderable leftPanel,
            IRenderable middlePanel,
            IRenderable rightPanel)
        {
            _renderer = new ScreenRenderer();
            _leftPanel = leftPanel;
            _middlePanel = middlePanel;
            _rightPanel = rightPanel;
        }

        public void Render() =>
            _renderer.DrawScreen(_leftPanel, _middlePanel, _rightPanel);

        public void ShowGameOver() => GameOverScreen.Show();
    }
}