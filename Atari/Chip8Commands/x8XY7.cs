namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't
    /// </summary>
    public class x8XY7 : BaseCommand
    {
        int X;
        int Y;

        public x8XY7(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
        }

        public override void Execute()
        {
            if (V[X] > V[Y])  // VY-VX
            {
                V[0xF] = 0; // there is a borrow
            }
            else
            {
                V[0xF] = 1;
            }

            V[X] = (byte)(V[Y] - V[X]);
            PC += 2;
        }
    }
}
