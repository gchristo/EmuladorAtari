namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Fills V0 to VX with values from memory starting at address I
    /// </summary>
    public class xFX65 : BaseCommand
    {
        int X;

        public xFX65(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            for (int i = 0; i <= (X); ++i)
            {
                V[i] = Chip.Memory[I + i];
            }

            // On the original interpreter, when the operation is done, I = I + X + 1.
            I += (ushort)(X + 1);
            PC += 2;
        }
    }
}
