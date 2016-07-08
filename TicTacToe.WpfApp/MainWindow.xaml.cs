using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe.WpfApp
{
    using Classes;
    using Enums;
    using TicTacToe.Classes;
    using TicTacToe = TicTacToe.Classes.TicTacToe;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEnumerable<Button> _buttons;

        public MainWindow()
        {
            InitializeComponent();

            var userInput = new TicTacToeUserWpfInput { CellType = TicTacToeCellType.Cross };
            var random = new Random((int)DateTime.Now.Ticks);
            var p2 = new TicTacToePlayer(userInput);
            var buttons = new List<Button>();

            buttons.Add(Button1);
            buttons.Add(Button2);
            buttons.Add(Button3);
            buttons.Add(Button4);
            buttons.Add(Button5);
            buttons.Add(Button6);
            buttons.Add(Button7);
            buttons.Add(Button8);
            buttons.Add(Button9);

            foreach (var button in buttons)
            {
                button.Click += (sender, args) =>
                {
                    var i = buttons.IndexOf(button);
                    userInput.SendInput(i / 3, i % 3);
                };
            }

            _buttons = buttons;
            var ttt = new TicTacToe(new TicTacToeWpfOutput(_buttons));
            ttt.GameEnded += OnGameEnded;

            var p1Input = new TicTacToeCpuInput(TicTacToeCpuDifficulty.Impossible, (r, c) => ttt[r, c]) { CellType = TicTacToeCellType.Circle };
            var p1 = new TicTacToePlayer(p1Input);
            p1.PlayerWaiting += sender => p1Input.GetInput();

            ttt.Start(p1, p2);
        }

        static void OnGameEnded(TicTacToe game, GameEndType end)
        {
            MessageBox.Show(end.ToString());
        }
    }
}