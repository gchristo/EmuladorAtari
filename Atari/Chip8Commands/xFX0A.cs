namespace Emulator.Chip8Commands
{
    /// <summary>
    /// A key press is awaited, and then stored in VX		
    /// </summary>
    public class xFX0A : BaseCommand
    {
        int X;

        public xFX0A(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            bool keyPress = false;

            for (int i = 0; i < 16; ++i)
            {
                if (Chip.Key[i] != 0)
                {
                    V[X] = (byte)I;
                    keyPress = true;
                }
            }

            // If we didn't received a keypress, skip this cycle and try again.
            if (!keyPress)
                return;

            PC += 2;
        }
    }
}
