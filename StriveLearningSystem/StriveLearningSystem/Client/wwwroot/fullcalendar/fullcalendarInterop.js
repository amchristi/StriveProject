
window.createNewCalendar = (selector, events) => {
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