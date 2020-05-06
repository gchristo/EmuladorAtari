using Emulator.Utils;

namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets VX to a random number and NN
    /// </summary>
    public class xCXNN : BaseCommand
    {
        int X;
        int NN;

        public xCXNN(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            NN = (opcode & 0x00FF);
        }

        public override void Execute()
        {
            V[X] = (byte)((Extensions.GetRandon(0xFF)) & NN);
            PC += 2;
        }
    }
}
