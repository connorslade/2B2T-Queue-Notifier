use iced::container;

mod dark;

pub enum Theme {
    Dark,
}

impl Default for Theme {
    fn default() -> Theme {
        Theme::Dark
    }
}

impl From<Theme> for Box<dyn container::StyleSheet> {
    fn from(theme: Theme) -> Self {
        match theme {
            Theme::Dark => dark::Container.into(),
        }
    }
}
