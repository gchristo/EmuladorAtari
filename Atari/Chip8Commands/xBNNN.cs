namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Jumps to the address NNN plus V0
    /// </summary>
    public class xBNNN : BaseCommand
    {
        ushort NNN;

        public xBNNN(ushort opcode, Chip8 chip) : base(chip) 
        {
            NNN = (ushort)(opcode & 0x0FFF);
        }

        public override void Execute()
        {
            PC = (ushort)(NNN + V[0]);
        }
    }
}