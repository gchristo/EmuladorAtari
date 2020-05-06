namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Jumps to address NNN
    /// </summary>
    public class x1NNN : BaseCommand
    {
        ushort address;

        public x1NNN(ushort opcode, Chip8 chip) : base(chip)
        {
            address = (ushort)(opcode & 0x0FFF);
        }

        public override void Execute()
        {
            PC = address;
        }
    }
}
