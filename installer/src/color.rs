// At this rate I should just make this colorprint stuff a crate..
// I copy it to almost every project I make now...

/// Define Text Colors
#[allow(dead_code)]
pub enum Color {
    Black,
    Red,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    White,
    Reset,
}

/// Get Color as an Integer.
#[rustfmt::skip]
fn get_color_code(color: Color) -> i32 {
    match color {
        Color::Black   => 30,
        Color::Red     => 31,
        Color::Green   => 32,
        Color::Yellow  => 33,
        Color::Blue    => 34,
        Color::Magenta => 35,
        Color::Cyan    => 36,
        Color::White   => 37,
        Color::Reset   => 0,
    }
}

/// Return string with ANSI color codes
pub fn color(text: &str, color: Color) -> String {
    format!("\x1B[0;{}m{}\x1B[0;0m", get_color_code(color), text)
}

/// Return string with ANSI color codes for bold text
pub fn color_bold(text: &str, color: Color) -> String {
    format!("\x1B[1;{}m{}\x1B[0m", get_color_code(color), text)
}

/// Color Print
///
/// Macro for *easy* printing of colored text to the console.
/// ## Example
/// ```rust
/// // A simple print
/// color_print!(Color::Green, "This is a green message!");
///
/// // A more complex print
/// color_print!(Color::Green, "This is a {} message!", "green");
/// ```
#[macro_use]
macro_rules! color_print {
    ($color:expr, $text:expr) => (
        println!("{}", color::color($text, $color))
    );
    ($color:expr, $($exp:expr),+) => (
        let mut text: String = "{}".to_string();
        $(text = text.replacen("{}", $exp, 1);)*
        println!("{}", color::color(&text, $color))
    );
}