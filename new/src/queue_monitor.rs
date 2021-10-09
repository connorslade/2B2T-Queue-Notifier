use iced::Subscription;
use iced_futures::futures;
use std::hash::{Hash, Hasher};

use crate::Message;

pub struct QueueMonitor<I> {
    id: I,
    queue: u32,
}

impl QueueMonitor<T> {
    pub fn subscription(&self) -> Subscription<Message> {

    }
}

impl<H, I, T> iced_native::subscription::Recipe<H, I> for QueueMonitor<T>
where
    T: 'static + Hash + Copy + Send,
    H: Hasher,
{
    type Output = T;

    fn hash(&self, state: &mut H) {
        struct Marker;
        std::any::TypeId::of::<Marker>().hash(state);

        self.id.hash(state);
    }

    fn stream(
        self: Box<Self>,
        input: iced_futures::BoxStream<I>,
    ) -> iced_futures::BoxStream<Self::Output> {
        let id = self.id;

        Box::pin(futures::stream::unfold(State::None, move |state| async move {
            Some((id, State::Queue(7)))
        }))
    }
}

enum State {
    None,
    Queue(u32),
}