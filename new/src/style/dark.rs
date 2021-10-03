use iced::{container, Color};

pub struct Container;

impl container::StyleSheet for Container {
    fn style(&self) -> container::Style {
        container::Style {
            background: Color::from_rgb8(46, 52, 64).into(),
            text_color: Color::WHITE.into(),
            ..container::Style::default()
        }
    }
}
