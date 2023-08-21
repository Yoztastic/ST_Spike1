namespace Sdk.Consumer;

public class ConsoleSpinner
{
    private static readonly string[,] Sequences;

    static ConsoleSpinner()
    {
        Sequences = new[,] {
            { "=>   ", "==>  ", "===> ", "====>" },
            { "/", "-", "\\", "|" },
            { ".", "o", "0", "o" },
            { "+", "x","+","x" },
            { "V", "<", "^", ">" },
            { ".   ", "..  ", "... ", "...." },
        };
    }

    public int Delay { get; set; } = 200;

    public int Cycles { get; set; } = 5;

    public int SequenceCode
    {
        get => _sequenceCode;
        set => _sequenceCode = value % Sequences.GetLength(0);
    }
    private int _sequenceCode;

    public async Task Turn(string displayMsg, CancellationToken cancellationToken)
    {
        var counter = new Counter();
        Console.ForegroundColor = ConsoleColor.Blue;
        while (counter.Cycle <= Cycles)
        {
            await Task.Delay(Delay,cancellationToken);
            Display(displayMsg + Sequences[SequenceCode, counter.NextSequenceItem]);
        }
        Display(string.Empty);
        ClearAnimation();
        Console.ResetColor();
    }

    private static void ClearAnimation() => Console.WriteLine();

    private static void Display(string fullMessage)
    {
        Console.Write(fullMessage);
        Console.SetCursorPosition(Console.CursorLeft - fullMessage.Length, Console.CursorTop);
    }

    private class Counter
    {
        private int counter;
        internal int Cycle { get; private set; }
        internal int NextSequenceItem
        {
            get
            {
                var counterValue = ++counter % 4;
                if (counterValue == 0)
                    Cycle++;
                return counterValue;
            }
        }
    }
}
