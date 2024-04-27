import kivy.app
import kivy.uix.label
class TestApp(kivy.app.App):

    def build(self):
        return kivy.uix.label.Label(text="Hello World")
app = TestApp()
app.run()