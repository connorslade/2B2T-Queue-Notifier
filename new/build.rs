use std::process::Command;

use chrono::Local;

fn main() {
    println!(
        "cargo:rustc-env=COMPILE_TIME={}",
        Local::now().format("%Y-%m-%d %H:%M:%S")
    );

    let commit_hash = quick_cmd("git", &["rev-parse", "HEAD"]);
    let branch = quick_cmd("git", &["branch", "--show-current"]);
    let dirty = !quick_cmd("git", &["status", "--porcelain"]).is_empty();
    println!(
        "cargo:rustc-env=GIT_INFO={} {}{}",
        commit_hash,
        branch,
        show_dirty(dirty)
    );

    #[cfg(windows)]
    {
        let mut res = winres::WindowsResource::new();
        res.set_icon("./assets/icon/icon.ico");
        res.compile().unwrap();
    }
}

fn quick_cmd(cmd: &str, args: &[&str]) -> String {
    String::from_utf8(Command::new(cmd).args(args).output().unwrap().stdout)
        .unwrap()
        .replace('\n', "")
        .replace('\r', "")
}

fn show_dirty(dirty: bool) -> &'static str {
    if dirty {
        return "*";
    }
    ""
}
