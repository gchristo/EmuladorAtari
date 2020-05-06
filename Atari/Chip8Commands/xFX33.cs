namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Stores the Binary-coded decimal representation of VX at the addresses I, I plus 1, and I plus 2
    /// </summary>
    public class xFX33 : BaseCommand
    {
        int X;

        public xFX33(ushort opcode, Chip8 chip) : base(chip)
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            Chip.Memory[I] = (byte)(V[X] / 100);
            Chip.Memory[I + 1] = (byte)((V[X] / 10) % 10);
            Chip.Memory[I + 2] = (byte)((V[X] % 100) % 10);
            PC += 2;
        }
    }
}
