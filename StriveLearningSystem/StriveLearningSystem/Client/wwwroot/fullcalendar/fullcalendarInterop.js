
window.createNewCalendar = (selector, events) => {
    //I'm doing this because passing in a string of events.
    events = JSON.parse(events);
    console.log(events);
    
    var calendarEl = document.getElementById(selector);

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['dayGrid'],
        events: events
    });

    calendar.render();

};

window.disposeCalendar = (selector) => {
    var calendarEl = document.getElementById(selector);
    calendarEl.destroy();
};