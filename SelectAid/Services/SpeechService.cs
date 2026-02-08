using System.Speech.Synthesis;
using SelectAid.Models;

namespace SelectAid.Services;

public class SpeechService
{
    private readonly SpeechSynthesizer _synth = new();

    public void Configure(SpeechSettings settings)
    {
        _synth.Rate = settings.Rate;
        _synth.Volume = settings.Volume;
        if (!string.IsNullOrWhiteSpace(settings.VoiceId))
        {
            _synth.SelectVoice(settings.VoiceId);
        }
    }

    public void Speak(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }
        _synth.SpeakAsyncCancelAll();
        _synth.SpeakAsync(text);
    }
}
