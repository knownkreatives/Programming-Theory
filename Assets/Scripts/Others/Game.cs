public static class GameError {
    public static string Error(int caseIndex) {
        string error = "";

        switch (caseIndex) {
            case 0:
            error = "Cannot load an empty or non-alphanumeric name";
            break;
            case 1:
            error = "Cannot save an empty or non-alphanumeric name";
            break;
        }

        return error;
    }
}