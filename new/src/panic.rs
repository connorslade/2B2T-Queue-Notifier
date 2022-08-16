use std::panic;
use std::process;

use native_dialog::MessageDialog;

use crate::{
    assets::{ERROR_REPORT, GITHUB_LINK},
    consts, VERSION,
};

pub fn set_handler() {
    panic::set_hook(Box::new(|p| {
        eprintln!("{}", p);
        let report = MessageDialog::new().set_title("2B2T-Queue-Notifier Fatal Error").set_text(&format!(
            "2B2T-Queue-Notifier encountered a fatal error!\nWould you like to report this problem?\n\n{}\n{}\nCompile Time: {}\nPlatform: {} {}\nVersion: {}",
            p,
            env!("GIT_INFO"),
            env!("COMPILE_TIME"),
            consts::OS,
            consts::ARCH,
            VERSION,
        )).set_type(native_dialog::MessageType::Error).show_confirm().unwrap_or_default();

        if report {
            let out = ERROR_REPORT
                .replace("{ERROR}", &p.to_string())
                .replace("{GIT}", env!("GIT_INFO"))
                .replace("{TIME}", env!("COMPILE_TIME"))
                .replace("{OS}", consts::OS)
                .replace("{ARCH}", consts::ARCH)
                .replace("{VERSION}", VERSION);
            open::that(format!(
                "{}/issues/new?labels=bug&body={}",
                GITHUB_LINK,
                urlencoding::encode(&out)
            ))
            .unwrap_or_default();
        }

        process::exit(-1);
    }));
}
