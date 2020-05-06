namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets VX to NN.
    /// </summary>
    public class x6XNN : BaseCommand
    {
        int X;
        byte NN;

        public x6XNN(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            NN = (byte)(opcode & 0x00FF);
        }

        public override void Execute()
        {
            V[X] =NN;
            PC += 2;
        }
    }
}
