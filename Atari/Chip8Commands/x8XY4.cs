namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't
    /// </summary>
    public class x8XY4 : BaseCommand
    {
        int X;
        int Y;

        public x8XY4(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
        }

        public override void Execute()
        {
            if (V[Y] > (0xFF - V[X]))
            {
                V[0xF] = 1; //carry
            }
            else
            {
                V[0xF] = 0;
            }

            V[X] += V[Y];
            PC += 2;
        }
    }
}
