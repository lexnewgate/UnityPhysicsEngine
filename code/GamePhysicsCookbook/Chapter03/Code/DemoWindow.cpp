#include "DemoWindow.h"
#include "imgui/imgui.h"
#include "CH14Demo.h"
#include "RaycastDemo.h"

// This is the global instance of the IWindow singleton!
static DemoWindow g_WindowInstance("Physics Sandbox", 800, 600);

void DemoWindow::OnInitialize() {
	GLWindow::OnInitialize();

	m_prevMousePos = vec2(0, 0);
	m_selectedDemo = -1;
	m_pDemo = 0;
	imgui_init = true;
}

void DemoWindow::OnResize(int width, int height) {
	GLWindow::OnResize(width, height);
	if (m_pDemo != 0) {
		m_pDemo->Resize(width, height);
	}
	ApplyDemoCamera();
}

void DemoWindow::OnRender() {
	GLWindow::OnRender();

	if (m_pDemo != 0) {
		mat4 view = m_pDemo->camera.GetViewMatrix();
		SetGLModelView(view.asArray);

		m_pDemo->Render();
	}
}

void DemoWindow::ApplyDemoCamera() {
	if (m_pDemo == 0) {
		return;
	}

	mat4 projection = m_pDemo->camera.GetProjectionMatrix();
	mat4 view = m_pDemo->camera.GetViewMatrix();

	SetGLProjection(projection.asArray);
	SetGLModelView(view.asArray);
}

void DemoWindow::OnUpdate(float deltaTime) {
	GLWindow::OnUpdate(deltaTime);

	if (imgui_init) {
		ImGui::SetNextWindowPos(ImVec2(10, 10));
		imgui_init = false;
	}
	ImGui::SetNextWindowSize(ImVec2(370, 75));
	ImGui::Begin("Physics Demo", 0, ImGuiWindowFlags_NoResize);
	ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);

	if (m_selectedDemo == 0) {
		if (ImGui::Button("Stop Chapter 14")) {
			m_selectedDemo = -1;
			StopDemo();
		}
	}
	else {
		if (ImGui::Button(" Run Chapter 14")) {
			m_selectedDemo = 0;
			Start14();
		}
	}
	/*ImGui::SameLine();
	if (m_selectedDemo == 1) {
		if (ImGui::Button("Stop Chapter 15")) {
			m_selectedDemo = -1;
			StopDemo();
		}
	}
	else {
		if (ImGui::Button(" Run Chapter 15")) {
			m_selectedDemo = 1;
			Start15();
		}
	}
	ImGui::SameLine();
	if (m_selectedDemo == 2) {
		if (ImGui::Button("Stop Chapter 16")) {
			m_selectedDemo = -1;
			StopDemo();
		}
	}
	else {
		if (ImGui::Button(" Run Chapter 16")) {
			m_selectedDemo = 2;
			Start16();
		}
	}*/

	ImGui::End();

	if (m_pDemo != 0) {
		m_pDemo->ImGUI();
	}
}

void DemoWindow::OnFixedUpdate(float deltaTime) {
	GLWindow::OnFixedUpdate(deltaTime);
	
	bool leftDown = MouseButonDown(MOUSE_LEFT);
	bool middleDown = MouseButonDown(MOUSE_MIDDLE);
	bool rightDown = MouseButonDown(MOUSE_RIGHT);

	vec2 mousePos = GetMousePosition();
	vec2 mouseDelta = mousePos - m_prevMousePos;
	mouseDelta.x /= (float)GetWidth();
	mouseDelta.y /= (float)GetHeight();

	if (m_pDemo != 0) {
		m_pDemo->SetMouseState(leftDown, middleDown, rightDown, mouseDelta, mousePos);
		m_pDemo->Update(deltaTime);
	}

	m_prevMousePos = mousePos;
}

void DemoWindow::OnShutdown() {
	GLWindow::OnShutdown();
	StopDemo();
}
void DemoWindow::StopDemo() {
	if (m_pDemo != 0) {
		m_pDemo->Shutdown();
		delete m_pDemo;
	}
	m_pDemo = 0;
}

void DemoWindow::Start14() {
	StopDemo();
	m_pDemo = new CH14Demo();
	m_pDemo->Initialize(GetWidth(), GetHeight());
	ApplyDemoCamera();
}

void DemoWindow::Start15() {
	StopDemo();
	// TODO
}

void DemoWindow::Start16() {
	StopDemo();
	// TODO
}