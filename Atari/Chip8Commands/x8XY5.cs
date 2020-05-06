namespace Emulator.Chip8Commands
{
    /// <summary>
    /// VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't
    /// </summary>
    public class x8XY5 : BaseCommand
    {
        int X;
        int Y;

        public x8XY5(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
        }

        public override void Execute()
        {
            if (V[Y] > V[X])
            {
                V[0xF] = 0; // there is a borrow
            }
            else
            {
                V[0xF] = 1;
            }

            V[X] -= V[Y];
            PC += 2;
        }
    }
}
