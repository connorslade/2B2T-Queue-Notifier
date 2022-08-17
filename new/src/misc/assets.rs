use iced::Font;

// Fonts
pub const MAIN_FONT_RAW: &[u8] = include_bytes!("../../assets/fonts/OpenSans-Regular.ttf");
pub const QUEUE_FONT: Font = Font::External {
    name: "JetBrainsMono-Medium.ttf",
    bytes: include_bytes!("../../assets/fonts/JetBrainsMono-Medium.ttf"),
};

// Images
pub const ICON: &[u8] = include_bytes!("../../assets/icon/icon.ico");

// Text
pub const ERROR_REPORT: &str = include_str!("../../assets/error_report.md");
pub const IGNORED_MESSAGES: [&str; 1] = ["Queued for server main."];
pub const GITHUB_LINK: &str = "https://github.com/Basicprogrammer10/2B2T-Queue-Notifier";
