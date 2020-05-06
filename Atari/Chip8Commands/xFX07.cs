namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets VX to the value of the delay timer
    /// </summary>
    public class xFX07 : BaseCommand
    {
        int X;

        public xFX07(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            V[X] = (byte)Timer_Delay;
            PC += 2;
        }
    }
}
