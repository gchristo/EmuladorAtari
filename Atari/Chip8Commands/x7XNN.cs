namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Adds NN to VX.
    /// </summary>
    public class x7XNN : BaseCommand
    {
        int X;
        byte NN;

        public x7XNN(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            NN = (byte)(opcode & 0x00FF);
        }

        public override void Execute()
        {
            V[X] += NN;
            PC += 2;
        }
    }
}
