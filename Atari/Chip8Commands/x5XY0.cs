namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Skips the next instruction if VX equals VY.
    /// </summary>
    public class x5XY0 : BaseCommand
    {
        int X;
        int Y;

        public x5XY0(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
        }

        public override void Execute()
        {
            if (V[X] == V[Y])
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
