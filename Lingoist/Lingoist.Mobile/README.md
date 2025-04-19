### Lingoist mobile app

This is a mobile app for Lingoist, a language learning platform. It allows users to learn languages through various methods, including flashcards, quizzes, and interactive exercises. The app is designed to be user-friendly and accessible, making it easy for users to learn on the go.

### Navigation

Due to limitations with Shell navigation, with little flexibility, we have implemented a custom navigation system: `ILingoNavigator`.
This supports page typed state management, page recycling, and custom transitions.

The goal is to optimize the app for performance and user experience, while maintaining a clean and maintainable codebase.

### Pages

Pages can either be stateless, or stateful.

Stateful pages should implement the ILingoStatefulPage interface while stateless pages should implement ILingoStatelessPage.

To navigate to stateful pages:
```csharp
MyPageState state = new();
Navigator.NavigateToAsync<MyPage, MyPageState>(state, LingoPageAnimation.SlideFromBottom);
```

To navigate to stateless pages:
```csharp
Navigator.NavigateToAsync<MyPage>(LingoPageAnimation.SlideFromBottom);
```

![demo](https://i.imgur.com/s3RdYpy.gif)
