namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets the delay timer to VX
    /// </summary>
    public class xFX15 : BaseCommand
    {
        int X;

        public xFX15(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            Timer_Delay = V[X];
            PC += 2;
        }
    }
}
