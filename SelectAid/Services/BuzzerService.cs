using System.Media;

namespace SelectAid.Services;

public class BuzzerService
{
    public void Buzz()
    {
        SystemSounds.Exclamation.Play();
    }
}
