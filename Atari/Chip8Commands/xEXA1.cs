namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Skips the next instruction if the key stored in VX isn't pressed
    /// </summary>
    public class xEXA1 : BaseCommand
    {
        int X;

        public xEXA1(ushort opcode, Chip8 chip) : base(chip)
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            if (Chip.Key[V[X]] == 0)
            {
                PC += 4;
            }
            else
            {
                PC += 2;
            }
        }
    }
}
