namespace Emulator.Chip8Commands
{
    public abstract class BaseCommand
    {
        protected Chip8 Chip;

        public BaseCommand(Chip8 chip)
        {
            Chip = chip;
        }

        public ushort PC
        {
            get { return Chip.PC; }
            set { Chip.PC = value; }
        }

        public ushort I
        {
            get { return Chip.I; }
            set { Chip.I = value; }
        }

        public byte[] V
        {
            get { return Chip.V; }
            set { Chip.V = value; }
        }

        public ushort[] Stack
        {
            get { return Chip.Stack; }
            set { Chip.Stack = value; }
        }

        public byte SP
        {
            get { return Chip.SP; }
            set { Chip.SP = value; }
        }

        public ushort Timer_Delay
        {
            get { return Chip.Timer_Delay; }
            set { Chip.Timer_Delay = value; }
        }

        public abstract void Execute();
    }
}
