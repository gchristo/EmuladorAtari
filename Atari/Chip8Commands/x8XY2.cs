namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets VX to "VX AND VY"
    /// </summary>
    public class x8XY2 : BaseCommand
    {
        int X;
        int Y;

        public x8XY2(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
        }

        public override void Execute()
        {
            V[X] &= V[Y];
            PC += 2;
        }
    }
}
