namespace Emulator.Chip8Commands
{
    /// <summary>
    ///  Clears the screen
    /// </summary>
    public class x00E0 : BaseCommand
    {
        public x00E0(Chip8 chip) : base(chip) { }

        public override void Execute()
        {
            Chip.ClearDisplay(true);
            PC += 2;
        }
    }
}
