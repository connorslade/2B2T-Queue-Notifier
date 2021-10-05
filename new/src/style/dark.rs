use iced::{
    button, checkbox, container, slider, slider::Handle, slider::HandleShape, text_input,
    Background, Color, Vector,
};

pub struct Container;
pub struct TextInput;
pub struct Button;
pub struct Slider;
pub struct Checkbox;

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

impl button::StyleSheet for Button {
    fn active(&self) -> button::Style {
        button::Style {
            background: Some(Background::Color(Color::from_rgb8(33, 37, 46))),
            text_color: Color::from_rgb8(242, 245, 252),
            border_radius: 5.0,
            border_width: 1.0,
            border_color: Color::from_rgb8(33, 37, 46),
            shadow_offset: Vector::new(0.5, 1.0),
        }
    }
}

impl slider::StyleSheet for Slider {
    fn active(&self) -> slider::Style {
        slider::Style {
            rail_colors: (Color::from_rgb8(41, 46, 57), Color::from_rgb8(40, 45, 56)),
            handle: Handle {
                shape: HandleShape::Rectangle {
                    width: 8,
                    border_radius: 4.0,
                },
                color: Color::from_rgb(0.95, 0.95, 0.95),
                border_color: Color::from_rgb8(33, 37, 46),
                border_width: 1.0,
            },
        }
    }

    fn hovered(&self) -> slider::Style {
        slider::Style {
            handle: Handle {
                color: Color::from_rgb(0.90, 0.90, 0.90),
                ..self.active().handle
            },
            ..self.active()
        }
    }

    fn dragging(&self) -> slider::Style {
        slider::Style {
            handle: Handle {
                color: Color::from_rgb(0.85, 0.85, 0.85),
                ..self.active().handle
            },
            ..self.active()
        }
    }
}

impl checkbox::StyleSheet for Checkbox {
    fn active(&self, _is_checked: bool) -> checkbox::Style {
        checkbox::Style {
            background: Background::Color(Color::from_rgb8(41, 46, 57)),
            checkmark_color: Color::WHITE,
            border_radius: 5.0,
            border_width: 1.0,
            border_color: Color::from_rgb8(33, 37, 46),
        }
    }

    fn hovered(&self, is_checked: bool) -> checkbox::Style {
        checkbox::Style {
            background: Background::Color(Color::from_rgb8(48, 54, 66)),
            ..self.active(is_checked)
        }
    }
}
