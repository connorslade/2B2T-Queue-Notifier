use iced::Font;

pub const MAIN_FONT_RAW: &[u8] = include_bytes!("../assets/fonts/OpenSans-Regular.ttf");

pub const QUEUE_FONT: Font = Font::External {
    name: "JetBrainsMono-Medium.ttf",
    bytes: include_bytes!("../assets/fonts/JetBrainsMono-Medium.ttf"),
};

pub const ICON: &[u8] = include_bytes!("../assets/icon/icon.ico");
