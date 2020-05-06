namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Adds VX to I
    /// </summary>
    public class xFX1E : BaseCommand
    {
        int X;

        public xFX1E(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            if (I + V[X] > 0xFFF)  // VF is set to 1 when range overflow (I+VX>0xFFF), and 0 when there isn't.
            {
                V[0xF] = 1;
            }
            else
            {
                V[0xF] = 0;
            }

            I += V[X];
            PC += 2;
        }
    }
}
