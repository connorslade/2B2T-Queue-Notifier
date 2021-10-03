use iced::{container, text_input, Background, Color};

pub struct Container;
pub struct TextInput;

impl container::StyleSheet for Container {
    fn style(&self) -> container::Style {
        container::Style {
            background: Color::from_rgb8(46, 52, 64).into(),
            text_color: Color::WHITE.into(),
            ..container::Style::default()
        }
    }
}

impl text_input::StyleSheet for TextInput {
    fn active(&self) -> text_input::Style {
        text_input::Style {
            background: Background::Color(Color::from_rgb8(41, 46, 57)),
            border_radius: 5.0,
            border_width: 1.0,
            border_color: Color::from_rgb8(33, 37, 46),
        }
    }

    fn focused(&self) -> text_input::Style {
        self.active()
    }

    fn placeholder_color(&self) -> Color {
        Color::from_rgb8(0, 0, 0)
    }

    fn value_color(&self) -> Color {
        Color::from_rgb8(242, 245, 252)
    }

    fn selection_color(&self) -> Color {
        Color::from_rgb(0.8, 0.8, 1.0)
    }
}
