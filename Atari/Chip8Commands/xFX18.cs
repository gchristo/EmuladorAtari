namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets the sound timer to VX
    /// </summary>
    public class xFX18 : BaseCommand
    {
        int X;

        public xFX18(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            Chip.Timer_Sound = V[X];
            PC += 2;
        }
    }
}
