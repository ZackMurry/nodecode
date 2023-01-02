extern "C" {
    void __main_designer();

    // This is literally the dumbest workaround ever
    // But, on Windows, I must've set up MinGW wrong or something, so `ld` kept on erroring out because
    // __chkstk wasn't defined... so I just defined it and it works.
    void __chkstk () {}
}

int main() {
    __main_designer();
    return 0;
}
