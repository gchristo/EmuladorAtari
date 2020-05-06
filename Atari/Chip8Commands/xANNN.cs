namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets I to the address NNN
    /// </summary>
    public class xANNN : BaseCommand
    {
        ushort NNN;

        public xANNN(ushort opcode, Chip8 chip) : base(chip) 
        {
            NNN = (ushort)(opcode & 0x0FFF);
        }

        public override void Execute()
        {
            I = NNN;
            PC += 2;
        }
    }
}
