using System.ComponentModel.DataAnnotations;

namespace NetEnhancements.Shared.Tests
{
    // ReSharper disable once ClassNeverInstantiated.Global, UnusedAutoPropertyAccessor.Global
    internal class FakeSettings
    {
        [Required]
        public string? Foo { get; set; }
    }
}
