use std::fs;

use chrono::Local;

fn main() {
    fs::create_dir_all("./build_data").unwrap();
    fs::write(
        "./build_data/compile_time",
        Local::now()
            .format("%Y-%m-%d %H:%M:%S")
            .to_string()
            .as_bytes(),
    )
    .unwrap();

    #[cfg(windows)]
    {
        let mut res = winres::WindowsResource::new();
        res.set_icon("./assets/icon/icon.ico");
        res.compile().unwrap();
    }
}
